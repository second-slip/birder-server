import { ObservationViewModel } from './ObservationViewModel';
import { ObservationFeedFilter } from './FilterFeedOptions';

export interface ObservationFeedDto {
    totalItems: number;
    // totalPages: number;
    items: ObservationViewModel[];
    returnFilter: ObservationFeedFilter;
    displayMessage: boolean;
    message: string;
}
