namespace Socializer.BlazorShared;

public class LayoutState
{
    public event Action? OnChange;
    private void NotifyStateChanged() => OnChange?.Invoke();

    private string _errorMessage = string.Empty;
    public bool HasError => !string.IsNullOrEmpty(ErrorMessage);
    public string ErrorMessage
    {
        get { return _errorMessage; }
        set
        {
            _errorMessage = value;
        }
    }

    private string _header = string.Empty;
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
