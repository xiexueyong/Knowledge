using System.Collections.Generic;

public class Triangle<T1,T2,T3>
{
    public T1 V1;
    public T2 V2;
    public T3 V3;

    public Triangle(T1 v1, T2 v2, T3 v3)
    {
        V1 = v1;
        V2 = v2;
        V3 = v3;
    }
    public Triangle(T1 v1, T2 v2)
    {
        V1 = v1;
        V2 = v2;
    }
    public Triangle(T1 v1)
    {
        V1 = v1;
    }

    public Triangle()
    {
    }
}
