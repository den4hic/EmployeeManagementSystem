import { Injectable } from '@angular/core';
import {ApiPaths} from "./enums/api-paths";
import {HttpClient} from "@angular/common/http";
import {TaskDto} from "./dtos/task.dto";
import {UserPhotoDto} from "./dtos/user-photo.dto";
import {FileUploadRequestDto} from "./dtos/file-upload-request.dto";

@Injectable({
  providedIn: 'root'
})
export class UserPhotoService {
  private apiPath : string = ApiPaths.UserPhoto;
  constructor(private http: HttpClient) { }

  deleteTask(userPhotoId: number) {
    const url = `${this.apiPath}/${userPhotoId}`;
    return this.http.delete(url);
  }

  updateUserPhoto(userPhoto: FileUploadRequestDto) {
    const url = `${this.apiPath}`;
    return this.http.put(url, userPhoto);
  }

  createUserPhoto(model: FileUploadRequestDto) {
    const url = `${this.apiPath}`;
    const formData: FormData = new FormData();
    formData.append('File', model.file);
    formData.append('UserId', model.userId.toString());
    return this.http.post(url, formData, {
      headers: {
      }
    });
  }
}
