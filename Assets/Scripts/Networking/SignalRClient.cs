using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using UnityEngine;

public class SignalRClient : MonoBehaviour
{
    private HubConnection connection;

    async void Start()
    {
        // Replace with your server address
        connection = new HubConnectionBuilder()
            .WithUrl("http://localhost:5000/gamehub") // Use https in production
            .WithAutomaticReconnect()
            .Build();

        // Handle messages from server
        connection.On<string>("PlayerJoined", (playerId) =>
        {
            Debug.Log($"Player joined: {playerId}");
        });

        connection.On<string, string>("CardPlayed", (playerId, card) =>
        {
            Debug.Log($"{playerId} played {card}");
        });

        try
        {
            await connection.StartAsync();
            Debug.Log("Connected to SignalR Hub!");

            // Test: join a game and play a card
            await connection.InvokeAsync("JoinGame", "room1");
            await connection.InvokeAsync("PlayCard", "room1", "Ace of Spades");
        }
        catch (Exception ex)
        {
            Debug.LogError($"SignalR connection error: {ex.Message}");
        }
    }

    private async void OnApplicationQuit()
    {
        if (connection != null)
            await connection.StopAsync();
    }
}
