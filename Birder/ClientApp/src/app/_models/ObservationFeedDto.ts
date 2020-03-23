import { ObservationViewModel } from './ObservationViewModel';
import { ObservationFeedFilter } from './ObservationFeedFilter';

export interface ObservationFeedDto {
    totalItems: number;
    // totalPages: number;
    items: ObservationViewModel[];
    returnFilter: ObservationFeedFilter;
    // displayMessage: boolean;
    // message: string;
}

export interface ObservationDto {
    totalItems: number;
    // totalPages: number;
    items: ObservationViewModel[];
    // returnFilter: ObservationFeedFilter;
    // displayMessage: boolean;
    // message: string;
}
