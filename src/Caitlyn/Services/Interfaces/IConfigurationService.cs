// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IConfigurationService.cs" company="Caitlyn development team">
//   Copyright (c) 2008 - 2013 Caitlyn development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Caitlyn.Services
{
    using Caitlyn.Models;

    public interface IConfigurationService
    {
        #region Methods
        Configuration LoadConfigurationForCurrentSolution();

        void SaveConfigurationForCurrentSolution();
        #endregion
    }
}