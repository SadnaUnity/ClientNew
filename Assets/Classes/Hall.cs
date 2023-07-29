using System.Collections.Generic;
using UnityEngine;


public class Hall
{
private Dictionary<int, RoomStatus> roomsDic;
    public Hall(HallDTO hallDTO)
    {
        this.roomsDic = new Dictionary<int, RoomStatus>();
        foreach (var roomStatusDTO in hallDTO.roomStatuses)
        {
            roomsDic.Add(roomStatusDTO.roomId,new RoomStatus(roomStatusDTO));
            
        }

    }

    public Dictionary<int, RoomStatus> GetAllRooms()
    {
        return roomsDic;
    }
    public Dictionary<int, string> getRoomsForHall()
    {
        Dictionary<int, string> selectedRooms = new Dictionary<int, string>();

        List<int> roomKeys = new List<int>(roomsDic.Keys);
        int roomsToSelect = Mathf.Min(6, roomKeys.Count);
        
        // Create a random number generator
        System.Random random = new System.Random();
        
        while (selectedRooms.Count < roomsToSelect)
        {
            // Generate a random index within the range of available rooms
            int randomIndex = random.Next(roomKeys.Count);

            // Get the room key and name based on the random index
            int randomKey = roomKeys[randomIndex];
            RoomStatus roomStatus = roomsDic[randomKey];
            
            // Add the room to the selectedRooms dictionary
            selectedRooms.Add(randomKey, roomStatus.GetRoomName());

            // Remove the selected room's key from the keys list to avoid duplicates
            roomKeys.RemoveAt(randomIndex);
        }

        return selectedRooms;
    }
}

