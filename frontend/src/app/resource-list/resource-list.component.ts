import { Component } from '@angular/core';
import { resource } from '../Models/resource';
import { ResourcesService } from '../service/resource.service';

@Component({
  selector: 'app-resource-list',
  templateUrl: './resource-list.component.html',
  styleUrls: ['./resource-list.component.css']
})
export class ResourceListComponent {
 resources: resource[] = [];
 //data: any;

  constructor(private resourceService: ResourcesService) {}

  ngOnInit(): void {
    debugger
    this.resourceService.getAllResources().subscribe(resource => {
      this.resources = resource;
    });
  }

}

