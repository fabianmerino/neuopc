using System;

namespace neulib
{
    public enum MsgType
    {
        Ping,
        Pong,
        DAHostsReq,
        DAHostsRes,
        DAServersReq,
        DAServersRes,
        DAConnectReq,
        DAConnectRes,
        DAConnectTestReq,
        DAConnectTestRes,
        DAStatusReq,
        DAStatusRes,
        DADataReq,
        DADataRes,
        DADisconnectReq,
        DADisconnectRes,
        DABrowseReq,
        DABrowseRes,
        UAStartReq,
        UAStartRes,
        UAStatusReq,
        UAStatusRes,
        UAStopReq,
        UAStopRes,
        ExitReq,
        ExitRes,
    }

    public enum MsgError
    {
        MsgInvalid,
        MsgSequenceError,
    }

    [Serializable]
    public class MsgBase
    {
        public MsgType Type;
        public int Error;
    }
}
