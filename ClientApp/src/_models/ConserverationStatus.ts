import { BirdDetailViewModel } from './BirdDetailViewModel';

export interface ConserverationStatus {
    ConserverationStatusId: number;

    ConservationStatus: string;

    Description: string;

    CreationDate: Date | string;

    LastUpdateDate: Date | string;

    Birds: BirdDetailViewModel[];
}
