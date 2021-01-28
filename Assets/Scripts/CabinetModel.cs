using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;
using Newtonsoft.Json;



namespace Assets.Scripts
{
    [System.Serializable]
    public class BriefItem
    {
        public BriefItem() { }
        public BriefItem(string id, string value)
        {
            Id = id;
            Value = value;
        }
        public string Id;
        public string Value;

        public virtual string GetLabel()
        {
            return Id ?? "";
        }
    }

    [System.Serializable]
    public class IoItem : BriefItem
    {
        //public int Id ;

        // ex. "r"|"w"|"rw"
        public IoItem()
        {

        }
        public IoItem(IoItem item)
        {
            this.Id = item.Id;
            this.Value = item.Value;
            this.Direction = item.Direction;
            this.ValueType = item.ValueType;
            this.ContainerId = item.ContainerId;
            this.Type = item.Type;
            this.DeviceId = item.DeviceId;
            this.DeviceChannel = item.DeviceChannel;
            this.ScheduleId = item.ScheduleId;
            this.Name = item.Name;
            this.Range = item.Range;
            this.Description = item.Description;
        }
        public string Direction;

        // ex. "boolean"|"integer"|"string"
        public string ValueType;

        public string ContainerId;

        public string Type;


        public string DeviceId;
        public int? DeviceChannel;
        public string ScheduleId;
        public string Name;
        public string Range;
        public string Description;

        public override string GetLabel()
        {
            if (!string.IsNullOrEmpty(Name))
                return Name;
            if (!string.IsNullOrEmpty(Type))
                return Type;
            return base.GetLabel();
        }
    }
    public static class ExtRequest
    {
        public static bool IsSuccess(this UnityWebRequest request)
        {
            if (request.isNetworkError) { return false; }

            if (request.responseCode == 0) { return true; }
            if (request.responseCode == (long)System.Net.HttpStatusCode.OK) { return true; }

            return false;
        }
    }

    [System.Serializable]
    public class ContainerItem
    {
        public string Id;
        public string Type;
        public string ParentId;
        public string Name;
        public string Position; //default is front, back, left, right, top
        public float X;
        public float Y;
        public float Z; //future use
        public float Width;
        public float Height;
        public float Length; //use width, length when drawing a drawer as parent

      
        public string GetLabel()
        {
            if (!string.IsNullOrEmpty(Name))
                return Name;
            if (!string.IsNullOrEmpty(Position))
                return Position + (!string.IsNullOrEmpty(Type) ? " " + Type : "");
            if (!string.IsNullOrEmpty(Type))
                return Type + (!string.IsNullOrEmpty(Id) ? " (" + Id + ")" : "");
            return Id ?? "";
        }
    }

    [System.Serializable]

    public class DeviceItem
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public string Configuration { get; set; }
        public string ClassName { get; set; }
        public string Name { get; set; }
    }
   
}

