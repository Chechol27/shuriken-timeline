public interface IView<TViewModel>  where TViewModel : IViewModel
{
    public void Bind(TViewModel viewModel);
}
