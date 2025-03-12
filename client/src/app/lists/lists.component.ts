import { Component, inject, OnInit } from '@angular/core';
import { Member } from '../_models/member';
import { LikesService } from '../_services/likes.service';
import { MemberCardComponent } from "../members/member-card/member-card.component";
import { FormsModule } from '@angular/forms';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { ButtonsModule } from 'ngx-bootstrap/buttons';

@Component({
  selector: 'app-lists',
  standalone: true,
  imports: [ButtonsModule, FormsModule, MemberCardComponent, PaginationModule],
  templateUrl: './lists.component.html',
  styleUrl: './lists.component.css'
})
export class ListsComponent implements OnInit{
  private likesService = inject(LikesService);
  members: Member[] = [];
  predicate = 'liked';

  ngOnInit(): void {
    this.loadLikes();
  }

  getTitle(){
    switch(this.predicate){
      case 'liked': return 'Members you like';
      case 'likedBy': return 'Members who like you';
      default: return 'Mutual';
    }
  }

  loadLikes() {
    this.likesService.getLikes(this.predicate).subscribe({
      next: (members) => {
        this.members = members;
      },
      error: (error) => {
        console.log(error);
      },
    });
  }
}
