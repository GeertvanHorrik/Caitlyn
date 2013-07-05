// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IConfigurationService.cs" company="Catel development team">
//   Copyright (c) 2008 - 2012 Catel development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Caitlyn.Services
{
    using Models;

    public interface IConfigurationService
    {
        Configuration LoadConfigurationForCurrentSolution();
        void SaveConfigurationForCurrentSolution();
    }
}