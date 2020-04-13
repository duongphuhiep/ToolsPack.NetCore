using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Xml;
using ToolsPack.String;

namespace ToolsPack.Webservice
{
    /// <summary>
    /// Add log instruction to BeforeSendRequest and AfterReceiveReply
    /// </summary>
    public class LoggingMessageInspector : IClientMessageInspector
    {
        public LoggingMessageInspector(ILogger logger, LogLevel level = LogLevel.Trace)
        {
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            LogLevel = level;
        }
        public ILogger Logger { get; }
        public LogLevel LogLevel { get; }

        public void AfterReceiveReply(ref Message reply, object correlationState)
        {
            using (var buffer = reply.CreateBufferedCopy(int.MaxValue))
            {
                var document = GetDocument(buffer.CreateMessage());
                Logger.Log(LogLevel, "Response #{id} | {body}", correlationState, document.OuterXml);
                reply = buffer.CreateMessage();
            }
        }

        public object BeforeSendRequest(ref Message request, IClientChannel channel)
        {
            string id = StringGenerator.CreateRandomString(5, "abcdefghijklmnpqrstuvwxyz0123456789");
            using (var buffer = request?.CreateBufferedCopy(int.MaxValue))
            {
                var document = GetDocument(buffer.CreateMessage());
                Logger.Log(LogLevel, "Request #{id} | {body} | {endpointUrl}", id, document.OuterXml, channel?.RemoteAddress);

                request = buffer.CreateMessage();
                return id;
            }
        }

        private XmlDocument GetDocument(Message request)
        {
            XmlDocument document = new XmlDocument();
            using (MemoryStream memoryStream = new MemoryStream())
            {
                // write request to memory stream
                using (XmlWriter writer = XmlWriter.Create(memoryStream))
                {
                    request.WriteMessage(writer);
                    writer.Flush();
                }
                memoryStream.Position = 0;

                // load memory stream into a document
                document.Load(memoryStream);
            }

            return document;
        }
    }
}
