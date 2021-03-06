// Copyright 2014 Adrian Chlubek. This file is part of GTA Multiplayer IV project.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.
using System;
using System.Collections.Generic;
using MIVSDK;

namespace MIVServer
{
    public class ServerNPCDialog
    {
        public string caption;
        public uint id;
        public List<string> responses;
        public string text;

        private static Dictionary<uint, ServerNPCDialog> pool;

        public ServerNPCDialog(string caption, string text)
        {
            if (pool == null) pool = new Dictionary<uint, ServerNPCDialog>();
            this.caption = caption;
            this.text = text;
            this.id = findLowestFreeId();
            responses = new List<string>();
        }

        public delegate void onPlayerAnswerDialogDelegate(ServerPlayer player, byte choose);

        public event onPlayerAnswerDialogDelegate onPlayerAnswerDialog;

        public static void invokeResponse(ServerPlayer player, uint id, byte answer)
        {
            if (pool.ContainsKey(id) && pool[id].onPlayerAnswerDialog != null) pool[id].onPlayerAnswerDialog.Invoke(player, answer);
        }

        public void addResponse(string text)
        {
            responses.Add(text);
        }

        public void show(ServerPlayer player)
        {
            var bpf = new BinaryPacketFormatter(Commands.NPCDialog_show);
            bpf.Add(id);
            bpf.Add(caption);
            bpf.Add(text);
            bpf.Add(String.Join("\x01", responses.ToArray()));
            player.connection.write(bpf.getBytes());
        }

        private uint findLowestFreeId()
        {
            for (uint i = 1; i < uint.MaxValue; i++)
            {
                if (!pool.ContainsKey(i)) return i;
            }
            throw new Exception("No free ids");
        }
    }
}