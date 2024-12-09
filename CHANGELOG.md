# 5.0.0

- 更新`Rougamo.Fody`到`5.0.0`，将所有Attribute都设置为`Pooled`生命周期，优化GC。
- 移出`Rougamo.OpenTelemetry.Hosting`项目的多SDK版本支持，仅使用netstandard2.0。没有针对特定版本有特殊操作，使用netstandard2.0即可提供最佳兼容
- 使用`AdviceAttribute`指定需要的功能，减少织入的代码量