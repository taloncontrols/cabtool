using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;
using Newtonsoft.Json;

using Grpc.Core;
using GrpcCabinet;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public class CabinetServiceGrpc : MonoBehaviour
    {

        const string GRPC_CABINETSERVICE_URL = "localhost:5010";
        bool iosLoaded = false;
        bool containersLoaded = false;

        string targetUrl = GRPC_CABINETSERVICE_URL;

        // Keep track of what we got back

        public List<IoItem> ios;
        public List<ContainerItem> containers;

        public List<DeviceItem> devices;
        bool devicesLoaded = false;

        public string getUrl()
        {
            return targetUrl;
        }
        public void setUrl(string url)
        {
            targetUrl = url;
        }

        void Awake()
        {
            targetUrl = GRPC_CABINETSERVICE_URL;
            this.DoCheck();
        }

        async void getContainersFromServer()
        {
            var channel = new Channel(targetUrl, ChannelCredentials.Insecure);
            var client = new GrpcCabinet.Cabinet.CabinetClient(channel);
            var reply = await client.GetContainersAsync(
                              new CabinetRequest());
            Debug.Log("GetContainers: " + reply.Containers.Count);
            this.containers = new List<ContainerItem>();
            var mapper = new Mapper();
            foreach (var c in reply.Containers)
            {
                this.containers.Add(mapper.ToDto(c));
            }
            containersLoaded = true;

        }

        async void getIosFromServer()
        {
            var channel = new Channel(targetUrl, ChannelCredentials.Insecure);
            var client = new GrpcCabinet.Cabinet.CabinetClient(channel);
            var reply = await client.GetIosAsync(
                              new CabinetRequest());
            Debug.Log("GetIos: " + reply.Ios.Count);
            this.containers = new List<ContainerItem>();
            var mapper = new Mapper();
            foreach (var c in reply.Ios)
            {
                this.ios.Add(mapper.ToDto(c));
            }
            iosLoaded = true;
        }

        async void getDevicesFromServer()
        {
            var channel = new Channel(targetUrl, ChannelCredentials.Insecure);
            var client = new GrpcCabinet.Cabinet.CabinetClient(channel);
            var reply = await client.GetDevicesAsync(
                              new CabinetRequest());
            Debug.Log("GetDevices: " + reply.Devices.Count);
            this.containers = new List<ContainerItem>();
            var mapper = new Mapper();
            foreach (var c in reply.Devices)
            {
                if (!string.IsNullOrWhiteSpace(c.ClassName))
                    this.devices.Add(mapper.ToDto(c));
            }
            //devices = devices.Where(x => !string.IsNullOrWhiteSpace(x.ClassName)).ToList();
            devicesLoaded = true;
        }
        // Where to send our request

       



        async void DoCheck()
        {
            for (; ; )
            {
                try
                {
                    var channel = new Channel(targetUrl, ChannelCredentials.Insecure);
                    var client = new GrpcCabinet.Cabinet.CabinetClient(channel);
                    var reply = client.GetHealth(new CabinetRequest());
                    if (reply.IsHealthy)
                    {
                        break;
                    }

                }
                catch (RpcException e)
                {
                    Debug.Log($"GetHealth: {e.Status}");
                }
                await Task.Delay(2000);
            }
            try
            {
                getContainersFromServer();
                getIosFromServer();
                getDevicesFromServer();
            }
            catch (RpcException e)
            {
                Debug.Log($"DoCheck: {e.Status}");
            }
        }
        public bool IsReady()
        {
            return containersLoaded && iosLoaded && devicesLoaded;
        }



        public void ChangeByContainerId(string containerId, string value)
        {
            var io = ios.FirstOrDefault(n => n.ContainerId == containerId);
            if (io == null) return;
            Change(io.Id, value);

        }

        public void ChangeByDeviceId(string deviceId, string value)
        {
            var io = ios.FirstOrDefault(n => n.DeviceId == deviceId);
            if (io == null) return;
            Change(io.Id, value);

        }
        public async void Change(string id, string value)
        {
            var io = ios.FirstOrDefault(n => n.Id == id);
            if (io == null) return;

            io.Value = value;

            var item = new BriefMsg() { Id = id, Value = value };
            var items = new List<BriefMsg>();
            items.Add(item);
            var request = new IoDirectRequest();
            request.UpdateItems.AddRange(items);
            try
            {
                var channel = new Channel(targetUrl, ChannelCredentials.Insecure);
                var client = new GrpcCabinet.Cabinet.CabinetClient(channel);
                var reply = await client.UpdateIoDirectAsync(request);
                Debug.Log("UpdateIo: " + reply.Success);
            }
            catch (RpcException e)
            {
                Debug.Log($"UpdateIo: {e.Status}");
            }
        }


        public string GetDeviceType(int value)
        {
            var deviceItem = devices[value];
            var type = !string.IsNullOrEmpty(deviceItem.ClassName) ? deviceItem.ClassName : deviceItem.Type;
            return type;
        }


    }
}

