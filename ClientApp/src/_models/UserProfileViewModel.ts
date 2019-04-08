export interface UserProfileViewModel {
    userName: string;
    profileImage: string;
    registrationDate: Date | string;
    isLoggedInUser: boolean;
    isFollowing: boolean;
    followersCount: number;
    followingCount: number;
}
