﻿
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using mvcproj.Models;

namespace mvcproj.Reporisatory
{
    public class RoomReporisatory :IGenericReporisatory<Room>, IRoomReporisatory
    {
        Reservecotexet context;
        public RoomReporisatory (Reservecotexet context)
        {
            this.context = context;
        }
        public void Delete(int id)
        {
            Room room = GetById (id);
            context.Remove(room);
        }

        public List<Room> GetAll()
        {
           List<Room> rooms= context.Rooms.Include(r => r.RoomType).Where(r => !r.IsDeleted).ToList();
            
            return rooms;
        }

        public Room GetById(int id)
        {
            Room room = context.Rooms.FirstOrDefault(r => r.RoomID == id);
            return room;
        }

        //public IEnumerable<Room> GetRoomStatus()
        //{
        //   List<string> roomStatus = context.Rooms.Select(r=>r.Status).ToList();
        //    return roomStatus
        //}

        public void Insert(Room obj)
        {
            context.Rooms.Add(obj);
            Save();
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Update(Room obj)
        {
            context.Update(obj);
            Save();
        }


        //i make it to use in update Room Action in Room Page 
        public Room GetRoomDetailsById(int id)
        {
            return context.Rooms
                .Where(r => r.RoomID == id)
                .Include(r => r.Hotel)
                .Include(r => r.RoomType)
                .FirstOrDefault();
        }

        public List<Room> CheckAvailability(DateTime checkIn, DateTime checkOut, int roomTypeId, int capacity)
        {
            List<Room> availableRooms = context.Rooms
                 .Include(r => r.RoomType) 
                 .Include(r => r.Bookings)
                 .Where(r => r.TypeID == roomTypeId)
                 .Where(r => r.RoomType.Capacity >= capacity)
                 .Where(r => !r.Bookings.Any(b =>
                     b.CheckinDate < checkOut &&
                     b.CheckoutDate > checkIn
                 )).ToList();
            return availableRooms;
                

        }
    }
}
