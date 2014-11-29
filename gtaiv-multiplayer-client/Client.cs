﻿using GTA;
using MIVSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;

namespace MIVClient
{
    public partial class Client : Script
    {
        public static UpdateDataStruct currentData;
        public static Client instance;
        public ChatController chatController;
        public TcpClient client;
        public ClientState currentState = ClientState.Initializing;
        public KeyboardHandler keyboardHandler;
        public string nick;
        public NPCPedController npcPedController;
        public PlayerPedController pedController;
        public PerFrameRenderer perFrameRenderer;
        public PlayerVehicleController playerVehicleController;
        public ServerConnection serverConnection;
        public VehicleController vehicleController;
        public TeleportCameraController teleportCameraController;
        private byte internalCounter;
        public bool isCurrentlyDead;

        private Queue<Action> actionQueue;

        private Dictionary<int, GTA.Vector3> bindPoints;

        public Client()
        {
            isCurrentlyDead = false;
            internalCounter = 0;
            actionQueue = new Queue<Action>();
            instance = this;
            teleportCameraController = new TeleportCameraController(this);
            pedController = new PlayerPedController();
            vehicleController = new VehicleController();
            npcPedController = new NPCPedController();
            playerVehicleController = new PlayerVehicleController();
            chatController = new ChatController(this);
            keyboardHandler = new KeyboardHandler(this);
            currentState = ClientState.Initializing;
            Interval = 80;
            GTA.Timer gfxupdate = new Timer(1);
            gfxupdate.Tick += gfxupdate_Tick;
            gfxupdate.Start();
            this.Tick += new EventHandler(this.eventOnTick);
            currentState = ClientState.Disconnected;
            System.IO.File.WriteAllText("multiv-log.txt", "");
            BindConsoleCommand("savepos", Client_ScriptCommand);
            BindConsoleCommand("tp2wp", delegate(ParameterCollection Parameters)
            {
                Blip wp = GTA.Game.GetWaypoint();
                if (wp != null)
                {
                    var pos = wp.Position;
                    Player.TeleportTo(pos.X, pos.Y);
                }
            });
            perFrameRenderer = new PerFrameRenderer(this);
            MouseDown += Client_MouseDown;
            MouseUp += Client_MouseUp;
        }

        void gfxupdate_Tick(object sender, EventArgs e)
        {
            if (pedController != null && pedController.streamer != null) pedController.streamer.updateGfx();
            if (npcPedController != null && npcPedController.streamer != null) npcPedController.streamer.updateGfx();
            /*var test = (Vector2)World.WorldToScreenProject(new Vector3(-229.4026f, 261.9114f, 14.862f));
            if (test.X < 0) test.X = 0;
            if (test.Y < 0) test.Y = 0;
            if (test.X > Game.Resolution.Width) test.X = Game.Resolution.Width;
            if (test.Y > Game.Resolution.Height) test.Y = Game.Resolution.Height;
            //            chatController.writeChat(test.X + " " + test.Y);

            PerFrameRenderer.test_x = test.X;
            PerFrameRenderer.test_y = test.Y;*/
        }

        public void finishSpawn()
        {
            Game.FadeScreenIn(2000);
            Player.CanControlCharacter = true;
        }

        void Client_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                //Game.TimeScale = 1.0f;
                Game.Mouse.Enabled = false;
            }
        }

        void Client_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                //Game.TimeScale = 0.1f;
                Game.Mouse.Enabled = true;
            }
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                var vehicles = World.GetVehicles(Player.Character.Position, 80.0f);
                var peds = World.GetPeds(Player.Character.Position, 80.0f);
                Ped selectedPed = null;
                Vehicle selectecVehicle = null;
                foreach (var ped in peds)
                {
                    var projected = (Vector2)World.WorldToScreenProject(ped.Position);
                    if (Math.Abs((projected - new Vector2(e.PixelLocation.X, e.PixelLocation.Y)).Length()) < 30.0)
                    {
                        selectedPed = ped;
                        break;
                    }
                }
                foreach (var vehicle in vehicles)
                {
                    var projected = (Vector2)World.WorldToScreenProject(vehicle.Position);
                    if (Math.Abs((projected - new Vector2(e.PixelLocation.X, e.PixelLocation.Y)).Length()) < 30.0)
                    {
                        selectecVehicle = vehicle;
                        break;
                    }
                }
                if (selectedPed != null)
                {
                    selectedPed.ApplyForce(new Vector3(0, 0, 11.0f));
                }
                else if(selectecVehicle != null)
                {
                    selectecVehicle.ApplyForce(new Vector3(0, 0, 11.0f));
                }
            }
        }

        public static Client getInstance()
        {
            return instance;
        }

        public static void log(string text)
        {
            System.IO.File.AppendAllText("multiv-log.txt", text + "\r\n");
            //GTA.Game.DisplayText(text);
        }

        public void enqueueAction(Action action)
        {
            actionQueue.Enqueue(action);
        }

        public Player getPlayer()
        {
            return Player;
        }

        public Ped getPlayerPed()
        {
            return Player.Character;
        }

        public void saveBindPoint(int id)
        {
            if (bindPoints == null) bindPoints = new Dictionary<int, Vector3>();
            if (bindPoints.ContainsKey(id)) bindPoints[id] = getPlayerPed().Position;
            else bindPoints.Add(id, getPlayerPed().Position);
        }

        public void teleportToBindPoint(int id)
        {
            if (bindPoints == null) bindPoints = new Dictionary<int, Vector3>();
            if (bindPoints.ContainsKey(id)) getPlayerPed().Position = bindPoints[id];
        }

        public void updateAllPlayers()
        {
            for (int i = 0; i < serverConnection.playersdata.Keys.Count; i++)
            {
                var elemKey = serverConnection.playersdata.Keys.ToArray()[i];
                var elemValue = serverConnection.playersdata[elemKey];

                if (elemValue.client_has_been_set) continue;
                else elemValue.client_has_been_set = true;

                //bool in_vehicle = elemValue.vehicle_id > 0;
                var posnew = new Vector3(elemValue.pos_x, elemValue.pos_y, elemValue.pos_z - 1.0f);
                StreamedPed ped = pedController.getById(elemKey, elemValue.nick, posnew);
                try
                {
                    updateVehicle(elemKey, elemValue, ped);
                }
                catch (Exception ex)
                {
                    log("Failed updating streamed vehicle data for player " + ex.Message);
                }
                try
                {
                    updatePed(elemKey, elemValue, ped);
                }
                catch (Exception ex)
                {
                    log("Failed updating streamed ped data for player " + ex.Message);
                }
            }
        }

        private void Client_ScriptCommand(ParameterCollection Parameters)
        {
            if (Player.Character.isInVehicle())
            {
                System.IO.File.AppendAllText("saved.txt",
                    "api.createVehicle(" + Player.Character.CurrentVehicle.Model.ToString() + ", new Vector3(" + Player.Character.CurrentVehicle.Position.X + "f, " +
                    Player.Character.CurrentVehicle.Position.Y + "f, " +
                    Player.Character.CurrentVehicle.Position.Z + "f), new Quaternion(" +
                    Player.Character.CurrentVehicle.RotationQuaternion.X + "f, " +
                    Player.Character.CurrentVehicle.RotationQuaternion.Y + "f, " +
                    Player.Character.CurrentVehicle.RotationQuaternion.Z + "f, " +
                    Player.Character.CurrentVehicle.RotationQuaternion.W + "f)); //" +
                    (Parameters.Count > 0 ? String.Join(" ", Parameters.Cast<string>()) : "") + "\r\n");
            }
            else
            {
                System.IO.File.AppendAllText("saved.txt",
                    "pos = " + Player.Character.Position.X + "f, " +
                    Player.Character.Position.Y + "f, " +
                    Player.Character.Position.Z + "f; heading = " + Player.Character.Heading + "f; //" +
                    (Parameters.Count > 0 ? Parameters[0].ToString() : "") + "\r\n");
            }
            log("Saved");
        }

        /*
         * Structure of packet is as follows
         * 0x00               0x01               0x05
         * [(ushort)COMMAND_ID] [(int)DATA_LENGTH] [(mixed)DATA]
         *
         */

        private void eventOnTick(object sender, EventArgs e)
        {
            try
            {
                while (actionQueue.Count > 0)
                {
                    actionQueue.Dequeue().Invoke();
                }
            }
            catch (Exception ex)
            {
                log("Failed executing action queue with message " + ex.Message);
            }
            if (currentState == ClientState.Connected)
            {
                if (currentData == null) currentData = UpdateDataStruct.Zero;
                try
                {
                    Player.WantedLevel = 0;
                    UpdateDataStruct data = new UpdateDataStruct();
                    data.nick = nick;
                    if (Player.Character.isDead)
                    {

                    }
                    //log("my X is " + data.pos_x.ToString());
                    if (Player.Character.isInVehicle() && Player.Character.CurrentVehicle.GetPedOnSeat(VehicleSeat.Driver) == Player.Character)
                    {
                        Vector3 pos = Player.Character.CurrentVehicle.Position;
                        data.pos_x = pos.X;
                        data.pos_y = pos.Y;
                        data.pos_z = pos.Z;

                        Vector3 vel = Player.Character.CurrentVehicle.Velocity;
                        data.vel_x = vel.X;
                        data.vel_y = vel.Y;
                        data.vel_z = vel.Z;

                        data.acc_x = data.vel_x - currentData.vel_x;
                        data.acc_y = data.vel_y - currentData.vel_y;
                        data.acc_z = data.vel_z - currentData.vel_z;

                        Quaternion quat = Player.Character.CurrentVehicle.RotationQuaternion;
                        data.rot_x = quat.X;
                        data.rot_y = quat.Y;
                        data.rot_z = quat.Z;
                        data.rot_a = quat.W;

                        var lastrotvect = new Quaternion(currentData.rot_x, currentData.rot_y, currentData.rot_z, currentData.rot_a).ToRotation();
                        data.acc_rx = quat.ToRotation().X - lastrotvect.X;
                        data.acc_ry = quat.ToRotation().Y - lastrotvect.Y;
                        data.acc_rz = quat.ToRotation().Z - lastrotvect.Z;

                        data.vehicle_model = Player.Character.CurrentVehicle.Model.Hash;
                        data.vehicle_health = Player.Character.CurrentVehicle.Health;
                        data.vehicle_id = vehicleController.getByVehicle(Player.Character.CurrentVehicle).id;
                        data.ped_health = Player.Character.Health;
                        data.speed = Player.Character.CurrentVehicle.Speed;
                        data.heading = Player.Character.CurrentVehicle.Heading;
                        if (vehicleController.streamer.vehicles.Count(a => a.gameReference != null && a.gameReference == Player.Character.CurrentVehicle) > 0)
                        {
                            var cveh = vehicleController.streamer.vehicles.First(a => a.gameReference != null && a.gameReference == Player.Character.CurrentVehicle);
                            cveh.position = pos;
                            cveh.orientation = quat;
                        }
                        data.state = 0;
                    }
                    else
                    {
                        Vector3 pos = Player.Character.Position;
                        data.pos_x = pos.X;
                        data.pos_y = pos.Y;
                        data.pos_z = pos.Z;

                        Vector3 vel = Player.Character.Velocity;
                        data.vel_x = vel.X;
                        data.vel_y = vel.Y;
                        data.vel_z = vel.Z;

                        data.rot_x = Player.Character.Direction.X;
                        data.rot_y = Player.Character.Direction.Y;
                        data.rot_z = Player.Character.Direction.Z;
                        data.rot_a = 0;

                        data.vehicle_model = 0;
                        data.vehicle_health = 0;
                        // for passengers:)
                        data.vehicle_id = Player.Character.isInVehicle() ? vehicleController.getByVehicle(Player.Character.CurrentVehicle).id : 0;
                        data.ped_health = Player.Character.Health;
                        data.speed = 0;
                        data.heading = Player.Character.Heading;
                        data.weapon = Player.Character.Weapons.CurrentType.GetHashCode();
                        data.state = 0;
                        data.state |= Player.Character.isShooting ? PlayerState.IsShooting : 0;
                        data.state |= Game.isGameKeyPressed(GameKey.Aim) ? PlayerState.IsAiming : 0;
                        data.state |= Game.isGameKeyPressed(GameKey.Crouch) ? PlayerState.IsCrouching : 0;
                        data.state |= Game.isGameKeyPressed(GameKey.Jump) ? PlayerState.IsJumping : 0;
                        data.state |= Game.isGameKeyPressed(GameKey.Attack) ? PlayerState.IsShooting : 0;

                        data.state |= Player.Character.isInVehicle() && Player.Character.CurrentVehicle.GetPedOnSeat(VehicleSeat.RightFront) == Player.Character ? PlayerState.IsPassenger1 : 0;
                        data.state |= Player.Character.isInVehicle() && Player.Character.CurrentVehicle.GetPedOnSeat(VehicleSeat.LeftRear) == Player.Character ? PlayerState.IsPassenger2 : 0;
                        data.state |= Player.Character.isInVehicle() && Player.Character.CurrentVehicle.GetPedOnSeat(VehicleSeat.RightRear) == Player.Character ? PlayerState.IsPassenger3 : 0;
                    }
                    data.vstate = 0;
                    data.vstate |= Game.isGameKeyPressed(GameKey.MoveForward) ? VehicleState.IsAccelerating : 0;
                    data.vstate |= Game.isGameKeyPressed(GameKey.MoveBackward) ? VehicleState.IsBraking : 0;
                    data.vstate |= Game.isGameKeyPressed(GameKey.MoveLeft) ? VehicleState.IsSterringLeft : 0;
                    data.vstate |= Game.isGameKeyPressed(GameKey.MoveRight) ? VehicleState.IsSterringRight : 0;
                    data.vstate |= Player.Character.isGettingIntoAVehicle ? VehicleState.IsEnteringVehicle : 0;
                    data.vstate |= (data.state & PlayerState.IsPassenger1) != 0 || (data.state & PlayerState.IsPassenger2) != 0 || (data.state & PlayerState.IsPassenger3) != 0
                        ? VehicleState.IsAsPassenger : 0;

                    var bpf = new BinaryPacketFormatter(Commands.UpdateData);
                    bpf.add(data);
                    serverConnection.write(bpf.getBytes());

                    if (Player.Character.Health == 0 || Player.Character.isDead || !Player.Character.isAlive)
                    {
                        Player.Character.Die();
                        isCurrentlyDead = true;
                    }

                    if (isCurrentlyDead && !Player.Character.isDead && Player.Character.isAlive && Player.Character.Health > 0)
                    {
                        Game.FadeScreenOut(200);
                        isCurrentlyDead = false;

                        var bpf2 = new BinaryPacketFormatter(Commands.InternalClient_requestSpawn);
                        serverConnection.write(bpf2.getBytes());
                    }

                    currentData = data;
                    updateAllPlayers();
                    serverConnection.flush();

                }
                catch (Exception ex)
                {
                    log("Failed sending new player data with message " + ex.Message);
                }

                try
                {
                    vehicleController.streamer.update();
                    pedController.streamer.update();
                    npcPedController.streamer.update();
                }
                catch (Exception ex)
                {
                    log("Failed updating streamers with message " + ex.Message);
                }
                //writeChat("Wrote");
                //log("sent data");
                // process players
                //internalCounter++;
                //if (internalCounter > 3) internalCounter = 0;

            }
            else
            {
                try
                {
                    Player.Character.Health = Player.Character.Health > 100 ? 100 : Player.Character.Health + 8;
                    //Player.Character.Invincible = true;
                }
                catch (Exception ex)
                {
                    log("Failed setting player health " + ex.Message);
                }
            }
            if (currentState == ClientState.Connecting)
            {
                currentState = ClientState.Connected;

                var bpf = new BinaryPacketFormatter(Commands.Connect);
                bpf.add(nick);
                serverConnection.write(bpf.getBytes());

                Player.Model = new Model("F_Y_HOOKER_01");
                Player.NeverGetsTired = true;

                chatController.writeChat("Connected");
            }



            teleportCameraController.onUpdate();


        }
    }
}