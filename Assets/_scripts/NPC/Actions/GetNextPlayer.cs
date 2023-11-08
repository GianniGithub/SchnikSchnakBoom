using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace GellosGames
{
    public class GetNextPlayer :  NPCModeBehaviour
    {
        private static List<(PlayerID,Transform)> players;
        private List<PlayerDistances> distances;
        private PlayerDistances nextPlayer;

        public GetNextPlayer(NPCMode Mother) : base(Mother)
        {
            distances = new List<PlayerDistances>();
            if (GetNextPlayer.players == null)
            {
                players = new List<(PlayerID,Transform)>();
                foreach (var player in PlayerEvents.GetAllPlayerEventsEnumerable())
                {
                    player.StartListening(PlayerActions.OnKilled, PlayerKilled);
                    player.StartListening(PlayerActions.OnReSpawn, PlayerReSpawned);
                    players.Add((player.id,player.PlayerObject.transform));
                }
            }
        }
        static void PlayerReSpawned(MonoBehaviour arg0, PlayerEventArgs arg1)
        {
            var handle = PlayerEvents.GetPlayerEventsHandler(arg1.From);
            players.Add((arg1.From, handle.PlayerObject.transform));
        }
        static void PlayerKilled(MonoBehaviour arg0, PlayerEventArgs arg1)
        {
            for (int i = players.Count - 1; i >= 0; i--)
            {
                if (players[i].Item1 == arg1.From)
                {
                    players.RemoveAt(i);
                }
            }
        }
        public override void Update()
        {
            distances.Clear();
            foreach ((PlayerID, Transform) player in players)
            {
                PlayerDistances dis;
                dis.NextPlayer = player.Item2;
                dis.Distance = Vector3.Distance(player.Item2.position, Npc.transform.position);
                distances.Add(dis);
            }
            distances.Sort();
            nextPlayer = distances[0];

            if (this.nextPlayer.NextPlayer != nextPlayer.NextPlayer)
            {
                Npc.StateChanged((PlayerID)3);
            }
        }
    }
    public struct PlayerDistances
    {
        public float Distance;
        public Transform NextPlayer;
    }
    public interface 
}