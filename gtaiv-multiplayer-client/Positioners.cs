﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTA;
using GTA.Forms;
using System.Net;
using System.Net.Sockets;
using MIVSDK;
using System.Drawing;

namespace MIVClient
{
    public partial class Client : Script
    {
        public void prepareVehicle(Vehicle vehicle)
        {
            if (vehicle.Exists())
            {
                vehicle.EngineRunning = true;
                vehicle.InteriorLightOn = true;
                vehicle.HazardLightsOn = true;
                vehicle.Repair();
            }
        }

        public void prepareVehicle(StreamedVehicle vehicle)
        {
            if (vehicle.gameReference != null) prepareVehicle(vehicle.gameReference);
        }

        public void preparePed(Ped ped)
        {
            if (ped.Exists())
            {
                ped.Invincible = true;
                ped.WillFlyThroughWindscreen = false;
                ped.PreventRagdoll = true;
            }
        }

        public void prepareVehicle(StreamedPed ped)
        {
            if (ped.gameReference != null) preparePed(ped.gameReference);
        }

        public void updateVehicle(UpdateDataStruct data, StreamedPed ped = null)
        {
            if (data.vehicle_id > 0)
            {
                var posnew = new Vector3(data.pos_x, data.pos_y, data.pos_z);
                StreamedVehicle veh = vehicleController.getById(data.vehicle_id);
                if (veh != null)
                {
                    veh.position = posnew;
                    veh.orientation = new Quaternion(data.rot_x, data.rot_y, data.rot_z, data.rot_a);
                    if (veh.streamedIn)
                    {
                        if (ped != null && ped.streamedIn && !ped.gameReference.isInVehicle())
                        {
                            ped.gameReference.WarpIntoVehicle(veh.gameReference, VehicleSeat.Driver);
                        }

                        veh.gameReference.Position = posnew;
                        veh.gameReference.RotationQuaternion = veh.orientation;
                        veh.gameReference.Velocity = new Vector3(data.vel_x, data.vel_y, data.vel_z);
                    }
                }
            }
        }

        public void updatePed(UpdateDataStruct data, StreamedPed ped)
        {
            var posnew = new Vector3(data.pos_x, data.pos_y, data.pos_z - 1.0f);
            ped.position = posnew;
            ped.heading = data.heading;
            if (ped.streamedIn && data.vehicle_id == 0)
            {
                if (ped.gameReference.isInVehicle())
                {
                    ped.gameReference.CurrentVehicle.PassengersLeaveVehicle(true);
                }
                if (data.nick != null && data.nick.Length > 0)
                {
                    ped.gameReference.GiveFakeNetworkName(data.nick, System.Drawing.Color.Red);
                }
                
                float delta = posnew.DistanceTo(ped.gameReference.Position);
                Vector3 vdelta = posnew - ped.gameReference.Position;
                ped.gameReference.Position = posnew;
                ped.gameReference.Heading = data.heading;
                //ped.gameReference.Weapons.MP5.Ammo = 999;

                if ((data.state & PlayerState.IsShooting) != 0)
                {
                    ped.gameReference.ShootAt(posnew + vdelta);
                }

                //ped.gameReference.Velocity = new Vector3(elemValue.vel_x, elemValue.vel_y, elemValue.vel_z);
                //ped.gameReference.Task.ClearAllImmediately();
                ped.animator.playAnimation(PedAnimations.Walk);
                if (delta > 0.2f)
                {
                    
                }
                else
                {
                    //ped.animator.playAnimation(PedAnimations.StandStill);
                }
            }
        }

    }
}
