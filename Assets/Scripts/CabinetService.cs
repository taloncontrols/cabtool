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
    class CabinetService : MonoBehaviour
    {
        const string TEST_CABINETSERVICE_URL = "http://localhost:5009";
        public bool iosLoaded = false;
        public bool containersLoaded = false;

        string targetUrl = TEST_CABINETSERVICE_URL;

        // Keep track of what we got back
        string recentData = "";
        public string containersData = "";
        public string iosData = "";
        public List<IoItem> ios;
        public List<ContainerItem> containers;

        public List<DeviceItem> devices;
        public bool devicesLoaded = false;
        public string devicesData = "";

        public void getContainersFromServer()
        {
            string url = TEST_CABINETSERVICE_URL + "/containers";
            this.StartCoroutine(this.RequestRoutine(url, this.ContainersResponseCallback));
        }

        public void getIosFromServer()
        {
            string url = TEST_CABINETSERVICE_URL + "/ios/all";
            this.StartCoroutine(this.RequestRoutine(url, this.IosResponseCallback));
        }

        public void getDevicesFromServer()
        {
            string url = TEST_CABINETSERVICE_URL + "/devices";
            this.StartCoroutine(this.RequestRoutine(url, this.DevicesResponseCallback));
        }

        // Where to send our request

        void Awake()
        {
            //this.StartCoroutine(this.RequestRoutine(this.targetUrl, this.ResponseCallback));
            getContainersFromServer();
            getIosFromServer();
            getDevicesFromServer();
        }

        private IEnumerator RequestRoutine(string url, Action<string> callback = null)
        {
            // Using the static constructor
            var request = UnityWebRequest.Get(url);

            // Wait for the response and then get our data
            yield return request.SendWebRequest();
            var data = request.downloadHandler.text;

            // This isn't required, but I prefer to pass in a callback so that I can
            // act on the response data outside of this function
            if (callback != null)
                callback(data);
        }

        // Callback to act on our response data
        private void ResponseCallback(string data)
        {
            Debug.Log(data);
            recentData = data;
        }

        private void ContainersResponseCallback(string data)
        {
            Debug.Log(data);
            containersData = data;
            containersLoaded = true;
            //containers=JsonHelper.FromJson<ContainerItem>(data);
            containers = JsonConvert.DeserializeObject<List<ContainerItem>>(data);
        }

        private void IosResponseCallback(string data)
        {
            Debug.Log(data);
            iosData = data;
            iosLoaded = true;
            //ios = JsonHelper.FromJson<IoItem>(data);
            ios = JsonConvert.DeserializeObject<List<IoItem>>(data);
        }
        private void DevicesResponseCallback(string data)
        {
            Debug.Log(data);
            devicesData = data;
            devicesLoaded = true;
            //ios = JsonHelper.FromJson<IoItem>(data);
            devices = JsonConvert.DeserializeObject<List<DeviceItem>>(data);
            devices = devices.Where(x => !string.IsNullOrWhiteSpace(x.ClassName)).ToList();
        }
        public void ChangeByContainerId(string containerId, string value)
        {
            var io = ios.FirstOrDefault(n => n.ContainerId == containerId);
            if (io == null) return;
            Change(io.Id, value);

        }
        public void Change(string id, string value)
        {
            var io = ios.FirstOrDefault(n => n.Id == id);
            if (io == null)  return;

            io.Value = value;
            string url = TEST_CABINETSERVICE_URL + "/ios/direct";
            var item = new BriefItem(id, value);
            var items = new List<BriefItem>();
            items.Add(item);
            //string str = JsonUtility.ToJson(items);
            string str = JsonConvert.SerializeObject(items);
            this.WebPatchRequest(url, str);
        }

        public void ChangeIo(IoItem item)
        {
            string url = TEST_CABINETSERVICE_URL + "/ios/direct/peripheral";
            
            var items = new List<IoItem>();
            items.Add(item);
            //string str = JsonUtility.ToJson(items);
            string str = JsonConvert.SerializeObject(items);
            this.WebPatchRequest(url, str);
        }

        public string GetDeviceType(int value)
        {           
            var deviceItem = devices[value];
            var type = !string.IsNullOrEmpty(deviceItem.ClassName) ? deviceItem.ClassName : deviceItem.Type;
            return type;
        }
        public void WebPatchRequest(string url, string str)
        {
            Action<string> onPostEventSuccess = (result) =>
            {
                // Do something when web request returns success
                Debug.Log("Web request succesfull");
            };
            Action<string> onPostEventError = (result) =>
            {
                // Do something when web request returns error
                Debug.Log($"Error: {result}");
            };
            // Call a coroutine containing our request
            StartCoroutine(WebPatchRequestAsync(url, str, onPostEventError, onPostEventSuccess));
        }
        private IEnumerator WebPatchRequestAsync(string url, string str, Action<string> onDeleteRequestError, Action<string> onDeleteRequestSuccess)
        {
            //// Create request URL string
            //string url = $"https://mywebrequest.com/api/object/patch/";

            ////Create request body
            //Dictionary<string, string> requestBody = new Dictionary<string, string>()
            //{
            //    {"Property1", "Hello" },
            //    {"Property2", "There"}
            //};
            // Serialize body as a Json string
            //string requestBodyString = JsonUtility.ToJson(requestBody);

            // Convert Json body string into a byte array
            byte[] requestBodyData = System.Text.Encoding.UTF8.GetBytes(str);

            // Create new UnityWebRequest, pass on our url and body as a byte array
            UnityWebRequest webRequest = UnityWebRequest.Put(url, requestBodyData);
            // Specify that our method is of type 'patch'
            webRequest.method = "PATCH";

            // Set request headers i.e. conent type, authorization etc
            webRequest.SetRequestHeader("Content-Type", "application/json");
            //webRequest.SetRequestHeader("Authorization", "Bearer ABC-123");
            webRequest.SetRequestHeader("Content-length", (requestBodyData.Length.ToString()));

            // Set the default download buffer
            webRequest.downloadHandler = new DownloadHandlerBuffer();

            // Send the request itself
            yield return webRequest.SendWebRequest();

            // Check for errors
            if (webRequest.isNetworkError || webRequest.isHttpError)
            {
                // Invoke error action
                onDeleteRequestError?.Invoke(webRequest.error);
            }
            else
            {
                // Check when response is received
                if (webRequest.isDone)
                {
                    // Invoke success action
                    onDeleteRequestSuccess?.Invoke("Patch Request Completed");
                }
            }
        }
    }
}

