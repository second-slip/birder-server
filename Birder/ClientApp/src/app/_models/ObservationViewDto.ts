export interface ObservationsPagedDto {
    totalItems: number;
    items: ObservationViewDto[];
}

export interface ObservationViewDto {
    observationId: number;
    quantity: number;
    observationDateTime: string;
    birdId: number;
    species: string;
    englishName: string;
    username: string;
    creationDate: string;
    lastUpdateDate: string;
}