export interface ObservationFeedDto {
    observationId: number;
    quantity: number;
    observationDateTime: string;
    birdId: number;
    species: string;
    englishName: string;
    thumbnailUrl: string;
    latitude: number;
    longitude: number;
    formattedAddress: string;
    shortAddress: string;
    username: string;
    notesCount: number;
    creationDate: string;
    lastUpdateDate: string;
}
