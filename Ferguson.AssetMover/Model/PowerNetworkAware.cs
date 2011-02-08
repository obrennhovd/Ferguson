//--------------------------------------------------------------------------
//
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//
//  File: PowerAware.cs
//
//  Description: Monitors battery and AC line status to provide power-aware capabilities
//
//--------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;
using Microsoft.Win32;
using System.Net.NetworkInformation;

namespace Ferguson.AssetMover.Client.Model
{
    class PowerNetworkAware
    {
        public long maxLinkSpeed = 0, minLinkSpeed = 0, LOW_PERF_THRESHOLD = (10 * 1024 * 1024);
        public bool isNetworkAvailable = false;

        public  ManagedPower._ACLineStatus currentPowerStatus;
        public  string currentPowerLevel;
        public  byte currentBatteryPercentage;
        public  string currentChargingStatus;
        public  string currentPowerLabel;
        public string networkStatus;
        public string networkName;
        public NetworkInterfaceType networkType;
        public string currentNetworkLabel;


        public PowerNetworkAware()
        {
            EnumerateNetworks();
        }

        /// <summary>
        /// Update the display based on the current power state.
        /// Returns from 0-3 for empty-full
        /// </summary>
        public void UpdatePowerInfo()
        {
            string text = "";
            ManagedPower.SystemPowerStatus sysPowerStatus;

            // Get the power status of the system
            if (ManagedPower.GetSystemPowerStatus(out sysPowerStatus))
            {
                // Current power source - AC/DC
                currentPowerStatus = sysPowerStatus.ACLineStatus;
                text += "Power source: " + sysPowerStatus.ACLineStatus.ToString() + "\n";

                // Current power status
                text += "Power status: ";

                // Check for unknown
                if (sysPowerStatus.BatteryFlag == ManagedPower._BatteryFlag.Unknown)
                {
                    text += "Unknown";
                }
                else
                {
                    // Check if currently charging
                    bool fCharging = (ManagedPower._BatteryFlag.Charging ==
                        (sysPowerStatus.BatteryFlag & ManagedPower._BatteryFlag.Charging));

                    if (fCharging)
                    {
                        sysPowerStatus.BatteryFlag -= ManagedPower._BatteryFlag.Charging;
                    }

                    // Print out power level
                    // If the power is not High, Low, or Critical, report it as "Medium".
                    currentPowerLevel = (sysPowerStatus.BatteryFlag != 0 ?
                        sysPowerStatus.BatteryFlag.ToString() :
                                "Medium");
                    text += currentPowerLevel;

                    // Print out charging status
                    if (fCharging)
                    {
                        currentChargingStatus = ManagedPower._BatteryFlag.Charging.ToString();
                        text += " (" + ManagedPower._BatteryFlag.Charging.ToString() + ") ";
                    }
                }

                // Finally print the percentage of the battery life remaining.
                currentBatteryPercentage = sysPowerStatus.BatteryLifePercent;
                text += "\nBattery life remaining is " +
                        sysPowerStatus.BatteryLifePercent.ToString() + "%";
            }

            currentPowerLabel = text;
        }

        /// <summary>
        /// This is invoked when the network connection changes bewteen online
        /// and offline.  It iterates over the collection of connections to
        /// determine the available speed.  This is used to determine a high
        /// or low connection and to set the icon and tooltip accordingly.
        /// </summary>
        public void EnumerateNetworks()
        {
            long speed, max = 0, min = 0;
            isNetworkAvailable = NetworkInterface.GetIsNetworkAvailable();
            bool enumeratedNetworkAvailable = false;

            if (isNetworkAvailable)
            {
                NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();
                foreach (NetworkInterface iface in interfaces)
                {
                    if (iface.OperationalStatus == OperationalStatus.Up)
                    {
                        enumeratedNetworkAvailable = true;

                        // Only use live net connections
                        if (iface.NetworkInterfaceType != NetworkInterfaceType.Loopback &&
                            iface.NetworkInterfaceType != NetworkInterfaceType.Tunnel)
                        {
                            speed = iface.Speed;
                            if (speed < min) min = speed;

                            // Use the name/description of the fastest network availabe
                            if (speed > max)
                            {
                                max = speed;

                                networkName = iface.Description;
                                networkType = iface.NetworkInterfaceType;
                            }
                        }
                    }
                }

                if (enumeratedNetworkAvailable)
                {
                    if (max < LOW_PERF_THRESHOLD)
                    {
                        networkStatus = "Connectivity Lo";
                    }
                    else
                    {
                        networkStatus = "Connectivity Hi";
                    }

                    String mbSpeed = ((int)((float)(max/1000/1000))).ToString();
                    String networkTypeString = networkType.ToString();
                    // Special case Wireless networks to make them more readable
                    if (networkType == NetworkInterfaceType.Wireless80211)
                    {
                        networkTypeString = "Wireless (802.11)";
                    }

                    currentNetworkLabel = "Network: " + networkName + "\nType: " + networkTypeString + "\n" + networkStatus + " (" + mbSpeed + "mb/sec)";
                }
                else
                {
                    currentNetworkLabel = "No network connection available";
                    networkStatus = "Disconnected";
                }
            }
            else
            {
                currentNetworkLabel = "No network copnnection available";
                networkStatus = "Disconnected";
            }

            // Update with the new values
            maxLinkSpeed = max;
            minLinkSpeed = min;
        }
    }

    /// <summary>
    /// This class contains the definitions of the unmanaged methods,
    /// structs and enums that will be used in the call to the 
    /// unmanaged power API.
    /// </summary>
    public class ManagedPower
    {
        // GetSystemPowerStatus() is the only unmanaged API called.
        [DllImport("kernel32")]
        internal static extern bool GetSystemPowerStatus(out SystemPowerStatus sps);

        [StructLayout(LayoutKind.Sequential)]
        internal struct SystemPowerStatus
        {
            public _ACLineStatus ACLineStatus;
            public _BatteryFlag BatteryFlag;
            public byte BatteryLifePercent;
            public byte Reserved1;
            public uint BatteryLifeTime;
            public uint BatteryFullLifeTime;
        }

        public enum _ACLineStatus : byte
        {
            Battery = 0,
            AC = 1,
            Unknown = 255
        }

        internal enum _BatteryFlag : byte
        {
            High = 1,
            Low = 2,
            Critical = 4,
            Charging = 8,
            NoSystemBattery = 128,
            Unknown = 255
        }
    }
}
