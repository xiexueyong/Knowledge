using System;
public class HttpPacket
{
	
   
    private bool _needSession;
    public bool needSession
    {
        get
        {
            return _needSession;
        }
    }

    private Body _body;
    public Body body
	{
		get { return _body; }
	}

    private Header _header;
    public Header header
    {
        get { return _header; }
    }

    private Action<string> _handler;
	public Action<string> handler
	{
		get { return _handler; }
        set { _handler = value; }
    }

    private Action<string> _errorHandler;
    public Action<string> errorHandler
    {
        get { return _errorHandler; }
        set { _errorHandler = value; }
    }


	public HttpPacket(Body __body, Action<string> __handler, Action<string> __errorHandler = null, Header __header = null,bool __needSession = true)
	{
		_body = __body;
        _handler = __handler;
        _errorHandler = __errorHandler;
        _header = __header;
        _needSession = __needSession;
    }
}