﻿using ErrH.Tools.Drupal7Models.Entities;
using ErrH.Tools.Extensions;

namespace ErrH.Tools.Drupal7Models.DTOs
{
    public abstract class D7NodeMap<T> where T : D7NodeBase, new()
    {
        public int   nid   { get; set; }
        public int   vid   { get; set; }


        public D7NodeBase ToNodeDTO(int userID)
        {
            var dto = new T();
            dto.CopyValuesFrom(this);

            var node = D7FieldMapper.Map(dto);
            node.uid = userID;
            node.nid = this.nid;
            //node.vi
            return node;
        }
    }
}
