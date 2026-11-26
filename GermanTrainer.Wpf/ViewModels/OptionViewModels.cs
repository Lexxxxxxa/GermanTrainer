using GermanTrainer.Wpf.ViewModels.Base;

namespace GermanTrainer.Wpf.ViewModels;

///<summary> 
///der/die/das button 
///</summary>
public class ArticleOptionViewModel : ViewModelBase
{
    private bool _isSelected;

    public ArticleOptionViewModel(string article)
    {
        Article = article;
    }

    public string Article { get; }

    public bool IsSelected
    {
        get => _isSelected;
        set => SetProperty(ref _isSelected, value);
    }
}

///<summary>
///Answer option in the translation trainer
///</summary>
public class AnswerOptionViewModel : ViewModelBase
{
    private bool _isSelected;

    public AnswerOptionViewModel(string text)
    {
        Text = text;
    }

    public string Text { get; }

    public bool IsSelected
    {
        get => _isSelected;
        set => SetProperty(ref _isSelected, value);
    }
}

///<summary>
///Checkable item (word type or lesson on the settings page)
///</summary>
public class CheckableItemViewModel<T> : ViewModelBase
{
    private bool _isChecked;
    private string _label;

    public CheckableItemViewModel(T value, string label, bool isChecked)
    {
        Value = value;
        _label = label;
        _isChecked = isChecked;
    }

    public T Value { get; }

    public string Label
    {
        get => _label;
        set => SetProperty(ref _label, value);
    }

    public bool IsChecked
    {
        get => _isChecked;
        set => SetProperty(ref _isChecked, value);
    }
}
