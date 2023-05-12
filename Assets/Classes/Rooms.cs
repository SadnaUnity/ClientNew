using System.Collections.Generic;
using UnityEngine;


public class Rooms
{
private Dictionary<int, string> rooms;

    public Rooms(RoomsDTO roomsDTO)
    {
        rooms = roomsDTO.roomsData;
    }

    public Dictionary<int, string> getRoomsForHall()
    {
        Dictionary<int, string> selectedRooms = new Dictionary<int, string>();

        List<int> roomKeys = new List<int>(rooms.Keys);
        int roomsToSelect = Mathf.Min(6, roomKeys.Count);
        
        // Create a random number generator
        System.Random random = new System.Random();
        
        while (selectedRooms.Count < roomsToSelect)
        {
            // Generate a random index within the range of available rooms
            int randomIndex = random.Next(roomKeys.Count);

            // Get the room key and name based on the random index
            int randomKey = roomKeys[randomIndex];
            string roomName = rooms[randomKey];

            // Add the room to the selectedRooms dictionary
            selectedRooms.Add(randomKey, roomName);

            // Remove the selected room's key from the keys list to avoid duplicates
            roomKeys.RemoveAt(randomIndex);
        }

        return selectedRooms;
    }
}

