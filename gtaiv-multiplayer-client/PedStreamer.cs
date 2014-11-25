﻿using GTA;
using System;
using System.Collections.Generic;
using System.Linq;

//using MIVSDK;

namespace MIVClient
{
    public class StreamedPed
    {
        private PedStreamer streamer;
        public Vector3 position, direction;
        public float heading;
        public bool streamedIn;
        public Ped gameReference;
        public PedAnimationManager animator;
        public bool hasNetworkName;
        public string model, networkname;
        public Blip blip;

        public StreamedPed(PedStreamer streamer, string model, string networkname, Vector3 position, float heading)
        {
            this.streamer = streamer;
            this.position = position;
            this.heading = heading;
            this.networkname = networkname;
            this.model = model;
            direction = Vector3.Zero;
            streamedIn = false;
            hasNetworkName = false;
            streamer.add(this);
            animator = new PedAnimationManager(this);
        }

        public void delete()
        {
            streamer.delete(this);
        }
    }

    public class PedStreamer
    {
        private Client client;
        public List<StreamedPed> peds;

        public PedStreamer(Client client)
        {
            this.client = client;
            peds = new List<StreamedPed>();
        }

        public void add(StreamedPed ped)
        {
            peds.Add(ped);
        }

        public void delete(StreamedPed ped)
        {
            peds.Remove(ped);
        }


        public void update()
        {
            Vector3 playerPos = client.getPlayerPed().Position;
            foreach (StreamedPed ped in this.peds)
            {
                try
                {
                    float distance = playerPos.DistanceTo(ped.position);
                    //client.chatController.writeChat(playerPos.X.ToString() + " " + playerPos.Y.ToString() + " " + playerPos.Z.ToString() + " ");
                    if (distance < 300.0f)
                    {
                        if (!ped.streamedIn || ped.gameReference == null || !ped.gameReference.Exists())
                        {
                            ped.gameReference = World.CreatePed(ped.model, ped.position, RelationshipGroup.Player);
                            ped.blip = Blip.AddBlip(ped.gameReference);
                            ped.blip.Color = BlipColor.LightOrange;
                            ped.blip.Display = BlipDisplay.MapOnly;
                            ped.blip.Icon = BlipIcon.Misc_Objective;
                            ped.blip.Name = "Player";
                            ped.streamedIn = true;
                            ped.gameReference.Heading = ped.heading;
                            ped.gameReference.GiveFakeNetworkName(ped.networkname, System.Drawing.Color.White);
                            //ped.gameReference.Task.LookAt(playerPos, 9999);
                        }
                        else
                        {
                            if (ped.gameReference.Position.DistanceTo(ped.position) > 4.0f)
                            {
                                //ped.gameReference.Position = ped.position;
                                //ped.gameReference.Heading = ped.heading;
                                //ped.gameReference.Task.RunTo(ped.position, true);
                            }
                        }
                    }
                    else if (ped.streamedIn)
                    {
                        ped.blip.Delete();
                        ped.gameReference.Delete();
                        ped.gameReference = null;
                        ped.hasNetworkName = false;
                        ped.streamedIn = false;
                    }
                }
                catch (Exception e)
                {
                    client.chatController.writeChat("PEDSTREAMER: " + e.Message);
                }
            }/*
            var pedss = World.GetPeds(client.getPlayerPed().Position, 200.0f);
            foreach (Ped a in pedss) if (a.Exists() && a.isAlive && a != client.getPlayerPed() && peds.Count(ax => ax.gameReference != null && ax.gameReference == a) == 0) a.Delete();*/
        }
    }
}