import { BirdDetailViewModel } from './BirdDetailViewModel';

export interface ConserverationStatus {
    conserverationStatusId: number;
    conservationStatus: string;
    description: string;
    creationDate: Date | string;
    lastUpdateDate: Date | string;
    birds: BirdDetailViewModel[];
}
