using Newtonsoft.Json;
using PermitToWork.Models.Log;
using PermitToWork.Models.User;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace PermitToWork.Utilities
{
    class CapturingResponseFilter : Stream
    {
        private Stream _sink;
        private MemoryStream mem;

        public UserEntity user { get; set; }
        public int id_permit { get; set; }
        public string controllerName { get; set; }
        public string actionName { get; set; }
        public string comment { get; set; }
        public int extension { get; set; }

        public CapturingResponseFilter(Stream sink)
        {
            _sink = sink;
            mem = new MemoryStream();
        }

        public override bool CanRead
        {
            get { return _sink.CanRead; }
        }

        public override bool CanSeek
        {
            get { return _sink.CanSeek; }
        }

        public override bool CanWrite
        {
            get { return _sink.CanWrite; }
        }

        public override long Length
        {
            get { return _sink.Length; }
        }

        public override long Position
        {
            get
            {
                return _sink.Position;
            }
            set
            {
                _sink.Position = value;
            }
        }

        public override long Seek(long offset, SeekOrigin direction)
        {
            return _sink.Seek(offset, direction);
        }

        public override void SetLength(long length)
        {
            _sink.SetLength(length);
        }

        public override void Close()
        {
            _sink.Close();
            mem.Close();
        }

        public override void Flush()
        {
            _sink.Flush();

            string content = GetContents(new UTF8Encoding(false));
            if (content.First() == '{')
            {
                Dictionary<string, string> dic = JsonConvert.DeserializeObject<Dictionary<string, string>>(content);
                string status = dic["status"];
                if (status == "200")
                {
                    LogEntity log = new LogEntity();
                    log.generateLog(user, id_permit, controllerName, actionName, comment, extension);
                }
            }
            //YOU CAN STORE YOUR DATA TO YOUR DATABASE HERE
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return _sink.Read(buffer, offset, count);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            mem.Write(buffer, 0, count);
            _sink.Write(buffer, offset, count);
        }

        public string GetContents(Encoding enc)
        {
            var buffer = new byte[mem.Length];
            mem.Position = 0;
            mem.Read(buffer, 0, buffer.Length);
            return enc.GetString(buffer, 0, buffer.Length);
        }
    }
}