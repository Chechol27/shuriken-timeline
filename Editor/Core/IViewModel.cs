using System.Collections.Generic;

public interface IViewModel
{
    IViewModel Parent { get; set; }
    List<IViewModel> Children { get; set; }

    public void Add(IViewModel child)
    {
        Children?.Add(child);
    }

    public TAncestor QUp<TAncestor>() where TAncestor : IViewModel
    {
        IViewModel parent = this;
        do
        {
            if (parent is TAncestor ancestor) return ancestor;
            parent = parent.Parent;
        } while (parent != null);

        return default;
    }
}
