//------------------------------------------------------------
// <copyright file="IItemModel.cs" company="TrevyBurgess" >
// Free for use, modification and distribution
// </copyright>
// <Author>
// Trevy Burgess
// </Author>
//------------------------------------------------------------
namespace CyberFeedForward.Models;

using Microsoft.UI.Xaml.Media;

public interface IItemModel<TCategory> where TCategory: ICategoryModel
{
    int ID { get; }

    TCategory Category { get; }

    string PageTitle { get; }
    
    ImageSource Image { get; }
}
