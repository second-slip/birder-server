import { ObservationViewModel } from './ObservationViewModel';
import { ObservationFeedFilter } from './ObservationFeedFilter';

export interface ObservationFeedPagedDto {
    totalItems: number;
    // totalPages: number;
    items: ObservationFeedDto[];
    returnFilter: string;// ObservationFeedFilter;
    // displayMessage: boolean;
    // message: string;
}

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

export interface ObservationDto {
    totalItems: number;
    // totalPages: number;
    items: ObservationViewModel[];
    // returnFilter: ObservationFeedFilter;
    // displayMessage: boolean;
    // message: string;
}
