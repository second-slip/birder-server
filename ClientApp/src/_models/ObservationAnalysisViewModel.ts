export interface ObservationAnalysisViewModel {
    totalObservationsCount: number;
    uniqueSpeciesCount: number;
}

export interface TopObservationsAnalysisViewModel {
    topObservations: TopObservationsViewModel[];
    topMonthlyObservations: TopObservationsViewModel[];
}

export interface TopObservationsViewModel {
    birdId: number;
    name: string;
    count: number;
}
