import { BirdDetailViewModel } from './BirdDetailViewModel';

export interface ConservationStatus {
    conservationStatusId: number;
    conservationList: string;
    description: string;
    creationDate: Date | string;
    lastUpdateDate: Date | string;
    birds: BirdDetailViewModel[];
}
