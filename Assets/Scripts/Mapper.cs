using System;
using System.Collections.Generic;
using System.Text;

using entities = GrpcCabinet;
using dtos = Assets.Scripts;

namespace Assets.Scripts
{
	public class Mapper
	{
        public dtos.IoItem ToDto(entities.IoMsg src)
        {
            var dest = new dtos.IoItem();
            dest.Id = src.Id;
            dest.ValueType = src.ValueType;
            dest.ContainerId = src.ContainerId;
            dest.DeviceChannel = src.DeviceChannel;
            dest.DeviceId = src.DeviceId;
            dest.Direction = src.Direction;
            dest.ScheduleId = src.ScheduleId;
            dest.Name = src.Name;
            dest.Range = src.Range;
            dest.Description = src.Description;
            dest.Value = src.Value;
            dest.Type = src.Type;
            return dest;
        }

        public dtos.ContainerItem ToDto(entities.ContainerMsg src)
        {
            var dest = new dtos.ContainerItem();
            dest.Height = src.Height;
            dest.Id = src.Id;
            
            dest.Length = src.Length;
            dest.ParentId = src.ParentId;
            dest.Position = src.Position;
            dest.Type = src.Type;
            dest.Width = src.Width;
            dest.X = src.X;
            dest.Y = src.Y;
            dest.Z = src.Z;
            return dest;
        }

        public dtos.DeviceItem ToDto(entities.DeviceMsg src)
        {
            var dest = new dtos.DeviceItem();
            dest.Id = src.Id;
            dest.Name = src.Name;
            dest.ClassName = src.ClassName;
            dest.Configuration = src.Configuration;
            dest.Type = src.Type;
            return dest;
        }

    } // Mapper
}
