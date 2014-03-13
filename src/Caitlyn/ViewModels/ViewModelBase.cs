// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ViewModelBase.cs" company="Caitlyn development team">
//   Copyright (c) 2008 - 2014 Caitlyn development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Caitlyn.ViewModels
{
    public abstract class ViewModelBase : Catel.MVVM.ViewModelBase
    {
        protected ViewModelBase()
        {
            DeferValidationUntilFirstSaveCall = true;
        }
    }
}