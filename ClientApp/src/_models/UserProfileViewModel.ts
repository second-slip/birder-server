import { UserViewModel } from './UserViewModel';

export interface UserProfileViewModel {
    userName: string;
    profileImage: string;
    registrationDate: Date | string;
    isOwnProfile: boolean;
    isFollowing: boolean;
    followers: UserViewModel[];
    following: UserViewModel[];
}
