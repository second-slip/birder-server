﻿using Birder.Data;
using Birder.Data.Model;
using Birder.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Birder.Services
{

    public interface IObsSummaryService
    {
        Task<ObservationAnalysisViewModel> GOA(Expression<Func<Observation, bool>> predicate);
    }
    public class ObsSummaryService: IObsSummaryService
    {

        private readonly ApplicationDbContext _dbContext;
        public ObsSummaryService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ObservationAnalysisViewModel> GOA(Expression<Func<Observation, bool>> predicate)
        {
            var result = new ObservationAnalysisViewModel();

            var query = _dbContext.Observations
                .Include(y => y.Bird)
                .Include(au => au.ApplicationUser)
                .AsNoTracking()
                .AsQueryable();

            query = query.Where(predicate);

            result.TotalObservationsCount = await query.CountAsync();

            result.UniqueSpeciesCount = await query.Select(i => i.BirdId).Distinct().CountAsync();

            return result;
        }
    }
}