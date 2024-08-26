namespace Voracious.Interface;

public interface IProgressReader
{
    void SetNBooks(int nbooks);
    void SetCurrentBook(string title);
    void AddLog(string log);
}
