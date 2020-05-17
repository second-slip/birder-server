export interface BirdSummaryViewModel {
    birdId: number;
    species: string;
    englishName: string;
    // internationalName: string;
    // category: string;
    populationSize: string;
    btoStatusInBritain: string;
    thumbnailUrl: string;
    // songUrl: string;
    conservationStatus: string;
    conservationListColourCode: string;
    birderStatus: string;
}

// export interface BirdsDdlDto {
//     birdId: number;
//     species: string;
//     englishName: string;
// }

export interface BirdsDto {
    totalItems: number;
    items: BirdSummaryViewModel[];
}
