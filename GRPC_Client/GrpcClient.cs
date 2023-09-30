using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grpc.Core;

namespace Utility
{
    public sealed class GrpcClient
    {
        private static readonly GrpcClient instance = new GrpcClient();
        public Channel Channel { get; set; }
        static GrpcClient() 
        {
            
        }
        private GrpcClient() 
        {
            Channel = new Channel("127.0.0.1", 10011, ChannelCredentials.Insecure);
        }
        public static GrpcClient Instance
        {
            get
            {
                return instance;
            }
        }
    }
}
