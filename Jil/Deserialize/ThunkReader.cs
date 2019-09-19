namespace Jil.Deserialize
{
    delegate T StringThunkDelegate<T>(ref ThunkReader writer, int depth);

    internal ref partial struct ThunkReader
    {
        
    }
}
