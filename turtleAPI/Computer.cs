﻿using System;
using System.Linq;
using System.Threading;
using TurtlesBrain.Shared;

namespace turtleAPI
{
    /// <summary>
    /// represents a Computer in the Minecraft world.
    /// The Computer is the main block of ComputerCraft
    /// </summary>
    public class Computer
    {
        /// <summary>
        /// The Name of your Computer
        /// </summary>
        public string Label { get; private set; }

        /// <summary>
        /// represents a Computer in the Minecraft world.
        /// The Computer is the main block of ComputerCraft.
        /// This initialize the connection to your computer
        /// </summary>
        /// <param name="label">the label of the Computer</param>
        /// <param name="server">the API Server which holds the connection to the Minecraftserver</param>
        public Computer(string label, Server server)
        {
            Label = label;
            _server = server;
        }

        
        internal AutoResetEvent wait = new AutoResetEvent(false);
        public string returnString = "";
        private Server _server;

        public void OnMessage(string content)
        {
            returnString = content;
            wait.Set();
        }

        /// <summary>
        /// Sends a Command to this Computer
        /// </summary>
        /// <param name="command">The command to you wish to send</param>
        /// <returns></returns>
        public string Send(string command)
        {
            Console.WriteLine($"Sending {Label}: {command}");
            _server.WriteAsync(new ClientMessage { Label = Label, Command = command }).Wait();
            wait.WaitOne();
            return Server.Instance[Label].returnString;
        }

        public bool GetBool(string theString)
        {
            return bool.Parse(theString.Split('|')[1]);
        }

        public byte GetByte(string theString)
        {
            return byte.Parse(theString.Split('|')[1]);
        }

        public int GetInt(string theString)
        {
            return int.Parse(theString.Split('|')[1]);
        }

        public string GetReason(string theString)
        {
            return theString.Split('|')[2];
        }

        public string[] GetArray(string theString, int skipAmount)
        {
            return theString.Split('|').Skip(skipAmount).ToArray();
        }
    }
}