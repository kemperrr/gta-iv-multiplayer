﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTA;

namespace MIVClient
{
    public class PlayerVehicleController
    {
        Dictionary<uint, Vehicle> vehicles;

        public PlayerVehicleController()
        {
            vehicles = new Dictionary<uint, Vehicle>();
        }

        private uint findLowestFreeId()
        {
            for (uint i = 0; i < uint.MaxValue; i++)
            {
                if (!vehicles.ContainsKey(i)) return i;
            }
            throw new Exception("No free ids");
        }

        public Vehicle getById(byte id, int model, Vector3 position)
        {
            if (!vehicles.ContainsKey(id))
            {
                vehicles.Add(findLowestFreeId(), World.CreateVehicle(new Model(model), position));
                Client.log("Created vehicle instance");
            }
            return vehicles[id];
        }

        public void destroy(byte id)
        {
            if (vehicles.ContainsKey(id))
            {
              //  vehicles[id].Delete();
                vehicles.Remove(id);
            }
        }
    }
}
