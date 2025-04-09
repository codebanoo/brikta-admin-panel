using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//using NetFwTypeLib;
//using W.Firewall;

namespace FrameWork
{
    public static class SystemFireWallManagement
    {
        //public static Exception ModifyFireWall(string action = "deny", 
        //    string direction = "in", 
        //    string remoteAddresses = "127.0.0.1", 
        //    int protocol = 6,
        //    string localPorts = "",
        //    bool enabled = true, 
        //    string interfaceTypes = "all", 
        //    string name = "")
        //{
        //    try
        //    {
        //        /***************************/
        //        //W.Firewall.Rules.Add()
        //        /***************************/

        //        INetFwRule firewallRule = (INetFwRule)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FWRule"));

        //        #region Set Action For FireWall(deny,allow,max)
        //        switch (action)
        //        {
        //            case "deny":
        //                firewallRule.Action = NET_FW_ACTION_.NET_FW_ACTION_BLOCK;
        //                break;
        //            case "allow":
        //                firewallRule.Action = NET_FW_ACTION_.NET_FW_ACTION_ALLOW;
        //                break;
        //            default:
        //                firewallRule.Action = NET_FW_ACTION_.NET_FW_ACTION_MAX;
        //                break;
        //        }
        //        #endregion

        //        #region Set Direction For FireWall(in,out,max)
        //        switch (direction)
        //        {
        //            case "in":
        //                firewallRule.Direction = NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_IN;
        //                break;
        //            case"out":
        //                firewallRule.Direction = NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_OUT;
        //                break;
        //            default:
        //                firewallRule.Direction = NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_MAX;
        //                break;
        //        }
        //        #endregion

        //        firewallRule.RemoteAddresses = remoteAddresses;
        //        firewallRule.Protocol = protocol;
        //        firewallRule.LocalPorts = localPorts;
        //        firewallRule.Enabled = enabled;
        //        firewallRule.InterfaceTypes = interfaceTypes;
        //        firewallRule.Name = name;
        //        INetFwPolicy2 firewallPolicy = (INetFwPolicy2)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FwPolicy2"));
        //        firewallPolicy.Rules.Add(firewallRule);
        //        return null;
        //    }
        //    catch (Exception exc)
        //    {
        //        return exc;
        //    }
        //}
    }
}