using GTANetworkAPI;
using System.Collections.Generic;
using System.Linq;

public class AdminCommands : Script
{
    private List<Vector3> greenZones = new List<Vector3>();


    [Command("metp")]
    public void CmdMetp(Player player)
    {
        if (player.IsInVehicle)
            player.Vehicle.Position = player.Position.Around(2.0f); 
        else
            player.Position = player.Position.Around(2.0f);
    }

    [Command("tpc", GreedyArg = true)]
    public void CmdTpc(Player player, Player target)
    {
        if (target != null)
            player.Position = target.Position; 
        else
            player.SendChatMessage("Игрок не найден.");
    }

  
    [Command("gethere", GreedyArg = true)]
    public void CmdGetHere(Player player, Player target)
    {
        if (target != null)
            target.Position = player.Position;
        else
            player.SendChatMessage("Игрок не найден.");
    }

    [Command("gethereveh", GreedyArg = true)]
    public void CmdGetHereVeh(Player player, Player target)
    {
        if (target != null && target.IsInVehicle)
            target.Vehicle.Position = player.Position;
        else
            player.SendChatMessage("Игрок не найден или не в машине.");
    }

    [Command("inv")]
    public void CmdInv(Player player)
    {
        player.Transparency = player.Transparency == 255 ? 0 : 255; 
    }

    [Command("admins")]
    public void CmdAdmins(Player player)
    {
        string adminList = "Список администраторов: \n";
        foreach (Player p in NAPI.Pools.GetAllPlayers().Where(p => p.HasData("AdminLevel")))
        {
            adminList += $"{p.Name}\n";
        }
        player.SendChatMessage(adminList);
    }

    [Command("leaders")]
    public void CmdLeaders(Player player)
    {
        string leaderList = "Список лидеров: \n";
        foreach (Player p in NAPI.Pools.GetAllPlayers().Where(p => p.HasData("LeaderLevel")))
        {
            leaderList += $"{p.Name}\n";
        }
        player.SendChatMessage(leaderList);
    }

    [Command("kill", GreedyArg = true)]
    public void CmdKill(Player player, Player target)
    {
        if (target != null)
            target.Health = 0; 
        else
            player.SendChatMessage("Игрок не найден.");
    }

    [Command("cuff", GreedyArg = true)]
    public void CmdCuff(Player player, Player target)
    {
        if (target != null)
        {
            bool isCuffed = target.HasSharedData("IsCuffed");
            target.SetSharedData("IsCuffed", !isCuffed); 
            target.SendChatMessage(isCuffed ? "Вас освободили." : "Вы в наручниках.");
        }
        else
            player.SendChatMessage("Игрок не найден.");
    }


    [Command("revive", GreedyArg = true)]
    public void CmdRevive(Player player, Player target)
    {
        if (target != null)
        {
            target.Health = 100;
            player.SendChatMessage($"{target.Name} был воскрешен.");
        }
        else
            player.SendChatMessage("Игрок не найден.");
    }

    [Command("veh", GreedyArg = true)]
    public void CmdVeh(Player player, VehicleHash model)
    {
        Vehicle veh = NAPI.Vehicle.CreateVehicle(model, player.Position, player.Heading, 0, 0);
        veh.SetSharedData("Owner", player.Name);
        player.SetIntoVehicle(veh, (int)VehicleSeat.Driver);
    }

    [Command("delveh")]
    public void CmdDelVeh(Player player)
    {
        if (player.IsInVehicle)
            player.Vehicle.Delete();
        else
            player.SendChatMessage("Вы не в машине.");
    }

    [Command("vehs", GreedyArg = true)]
    public void CmdVehs(Player player, VehicleHash model, int count)
    {
        for (int i = 0; i < count; i++)
        {
            Vector3 spawnPos = player.Position.Around(5.0f * (i + 1));
            Vehicle veh = NAPI.Vehicle.CreateVehicle(model, spawnPos, player.Heading, 0, 0);
            veh.SetSharedData("Owner", player.Name);
        }
    }

    [Command("setgreenzone")]
    public void CmdSetGreenZone(Player player)
    {
        greenZones.Add(player.Position);
        player.SendChatMessage("Зеленая зона установлена.");
    }
    public bool IsInGreenZone(Player player)
    {
        return greenZones.Any(zone => player.Position.DistanceTo(zone) < 50.0f);
    }
}
