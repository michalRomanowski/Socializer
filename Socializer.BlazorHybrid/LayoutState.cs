namespace Socializer.BlazorHybrid;

internal class LayoutState
{
    public event Action? OnChange;
    private void NotifyStateChanged() => OnChange?.Invoke();

    private string _errorMessage;
    public bool HasError => !string.IsNullOrEmpty(ErrorMessage);
    public string ErrorMessage
    {
        get { return _errorMessage; }
        set
        {
            _errorMessage = value;
        }
    }

    private string _header;
    public string Header
    {
        get { return _header; }
        set
        {
            _header = value;
            NotifyStateChanged();
        }
    }
}
