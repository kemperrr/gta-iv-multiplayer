// Copyright 2014 Adrian Chlubek. This file is part of GTA Multiplayer IV project.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.
using MIVSDK;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace MIVServer
{
    public class TextView
    {
        public uint id;
        public Point position;
        public float size;
        public string text;

        private ServerPlayer player;

        public TextView(ServerPlayer player, string text, float fontsize, Point position)
        {
            if (textviewspool == null) textviewspool = new Dictionary<uint, TextView>();
            this.text = text;
            this.size = fontsize;
            this.position = position;
            this.player = player;
            id = findLowestFreeId();
            textviewspool.Add(id, this);

            var bpf = new BinaryPacketFormatter(Commands.TextView_create);
            bpf.Add(id);
            bpf.Add(size);
            bpf.Add(position.X);
            bpf.Add(position.Y);
            bpf.Add(text);
            player.connection.write(bpf.getBytes());
        }

        private static Dictionary<uint, TextView> textviewspool
        {
            get
            {
                if (textviewspool == null) textviewspool = new Dictionary<uint, TextView>();
                return textviewspool;
            }
            set
            {
                textviewspool = value;
            }
        }

        public void destroy()
        {
            textviewspool.Remove(id);
            var bpf = new BinaryPacketFormatter(Commands.TextView_destroy);
            bpf.Add(id);
            player.connection.write(bpf.getBytes());
        }

        public void update()
        {
            var bpf = new BinaryPacketFormatter(Commands.TextView_update);
            bpf.Add(id);
            bpf.Add(size);
            bpf.Add(position.X);
            bpf.Add(position.Y);
            bpf.Add(text);
            player.connection.write(bpf.getBytes());
        }

        private uint findLowestFreeId()
        {
            for (uint i = 1; i < uint.MaxValue; i++)
            {
                if (!textviewspool.ContainsKey(i)) return i;
            }
            throw new Exception("No free ids");
        }
    }
}