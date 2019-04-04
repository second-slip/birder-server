export interface LifeListViewModel {
    userName: string;
    lifeList: SpeciesSummaryViewModel[];
}

export interface SpeciesSummaryViewModel {
    birdId: number;
    englishName: string;
    species: string;
    populationSize: string;
    btoStatusInBritain: string;
    conservationStatus: string;
    count: number;
}
